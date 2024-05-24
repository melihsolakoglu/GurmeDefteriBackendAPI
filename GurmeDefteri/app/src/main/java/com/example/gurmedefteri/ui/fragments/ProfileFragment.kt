package com.example.gurmedefteri.ui.fragments

import android.content.Intent
import android.os.Bundle
import android.text.InputType
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.MotionEvent
import android.view.View
import android.view.ViewGroup
import android.widget.EditText
import androidx.fragment.app.viewModels
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.Observer
import androidx.navigation.fragment.findNavController
import com.example.gurmedefteri.R
import com.example.gurmedefteri.SplashScreen
import com.example.gurmedefteri.data.entity.NewUser
import com.example.gurmedefteri.databinding.FragmentProfileBinding
import com.example.gurmedefteri.ui.viewmodels.ProfileViewModel
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import dagger.hilt.android.AndroidEntryPoint
import dagger.hilt.android.ViewModelLifecycle

@AndroidEntryPoint
class ProfileFragment : Fragment() {
    private lateinit var binding: FragmentProfileBinding
    private lateinit var viewModel: ProfileViewModel

    private lateinit var signInPass: EditText
    private var isPasswordVisible: Boolean = false
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentProfileBinding.inflate(inflater, container,false)

        signInPass = binding.profilePassword

        signInPass.setOnTouchListener { v, event ->
            val DRAWABLE_END = 2
            if (event.action == MotionEvent.ACTION_UP) {
                if (event.rawX >= (signInPass.right - signInPass.compoundDrawables[DRAWABLE_END].bounds.width())) {
                    togglePasswordVisibility()
                    return@setOnTouchListener true
                }
            }
            false
        }

        binding.button3.setOnClickListener {
            viewModel.logOut()
        }
        viewModel.loggedOut.observe(viewLifecycleOwner){
            try {
                val loggedOut = it
                if(loggedOut){
                    val intent = Intent(activity, SplashScreen::class.java)
                    intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP or Intent.FLAG_ACTIVITY_NEW_TASK)
                    startActivity(intent)
                    activity?.finish()
                }
                else{

                }

            }catch (e:Exception){

            }
        }
        viewModel.areYouSure.observe(viewLifecycleOwner){
            if(it){
                alertSure("Hesabınız Silinsin Mi?","Bütün Bilgileriniz Silincek!!!")
            }
        }

        viewModel.user.observe(viewLifecycleOwner){
            try {
                binding.profileName.setText(it.name)
                binding.profileEmail.setText(it.email)
                binding.profileAge.setText(it.age.toString())
                viewModel.userId.value =it.id

            }catch (e:Exception){

            }
        }
        binding.button.setOnClickListener {
            viewModel.areYouSure.value =true
        }
        viewModel.getUserId.observe(viewLifecycleOwner, Observer { Id ->
            val UserId = Id
            viewModel.getUserById(UserId)
        })
        binding.buttonUpdateUser.setOnClickListener {
            var everythingOk = true
            var changePass = false
            val name = binding.profileName.text.toString()
            val email = binding.profileEmail.text.toString()
            val ageText = binding.profileAge.text.toString()
            var age = -5

            val pass = binding.profilePassword.text.toString()




            val regexForEmail = Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}\$")
            if(!(regexForEmail.matches(email))&&everythingOk){
                everythingOk = false
                alert("Email Hatası","Lütfen Email Adresinizi Kontrole Edin")
            }
            if(!(name.length >2) &&everythingOk){
                everythingOk = false
                alert("İsim Hatası","İsim En Az 2 Harf Olmalıdır")
            }
            if(!(name.length<50)&&everythingOk){
                everythingOk = false
                alert("İsim Hatası","İsim En Fazla 50 Harf Olmalıdır")
            }
            if(ageText !="" && everythingOk){
                age= ageText.toInt()
            }
            else{
                if(everythingOk){
                    alert("Yaş Hatası","Lütfen Yaşınızı Boş Bırakmayınız")
                    everythingOk =false
                }
            }
            val regexOnlyLetter = Regex("^[a-zA-Z]+$")
            if(!(regexOnlyLetter.matches(name))&&everythingOk){
                everythingOk = false
                alert("İsim Hatası","İsim Sadece Harflerden Oluşmalıdır")
            }
            if(age>0 && age>200 && everythingOk){
                everythingOk = false
                alert("Yaş Hatası","Yaşınız 200 den fazla olamaz")
            }
            if(pass !=""){
                changePass = true
                val regexForPass = Regex("^(?=.\\d)(?=.[A-Z])(?=.*[a-z]).{8,100}\$")
                if(!(regexForPass.matches(pass))){
                    everythingOk = false
                    alert("Parola Hatası","Parola en az 8 karakterden oluşmalı ve içinde şunlar bulunmalıdır: büyük harf, küçük harf ve sayı")
                }
            }

            if(everythingOk &&!changePass){
                viewModel.updateUser(name, email, age)
            }else if(everythingOk && changePass){
                viewModel.updateUser(name, email,pass, age)
            }

        }
        viewModel.updateSucces.observe(viewLifecycleOwner){
            if(it){
                alert("Kullanıcı Başarıyla Güncellendi","Kullanıcı Başarıyla Güncellendi")
            }
        }


        return binding.root
    }

    private fun togglePasswordVisibility() {
        if (isPasswordVisible) {
            // Şifreyi gizle
            signInPass.inputType = InputType.TYPE_CLASS_TEXT or InputType.TYPE_TEXT_VARIATION_PASSWORD
            signInPass.setCompoundDrawablesWithIntrinsicBounds(R.drawable.ic_lock_login, 0, R.drawable.password_eye_2, 0)
        } else {
            // Şifreyi göster
            signInPass.inputType = InputType.TYPE_CLASS_TEXT
            signInPass.setCompoundDrawablesWithIntrinsicBounds(R.drawable.ic_lock_login, 0, R.drawable.password_eye, 0)
        }
        // İmleç konumunu koru
        signInPass.setSelection(signInPass.text.length)
        isPasswordVisible = !isPasswordVisible
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val tempViewModel: ProfileViewModel by viewModels()
        viewModel = tempViewModel
    }

    fun alert(title:String, message:String){
        MaterialAlertDialogBuilder(requireContext())
            .setTitle(title)
            .setMessage(message)
            .setPositiveButton("Tamam "){d,i ->
            }
            .show()
    }
    fun alertSure(title:String, message:String){
        MaterialAlertDialogBuilder(requireContext())
            .setTitle(title)
            .setMessage(message)
            .setPositiveButton("EVET "){d,i ->
                viewModel.deleteUser()
            }
            .setTitle(title)
            .setMessage(message)
            .setNegativeButton("HAYIR "){d,i ->
                viewModel.areYouSure.value=false
            }
            .show()
    }

}