package com.example.gurmedefteri.ui.fragments

import android.os.Bundle
import android.text.InputType
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.MotionEvent
import android.view.View
import android.view.ViewGroup
import android.widget.EditText
import androidx.fragment.app.viewModels
import androidx.navigation.fragment.findNavController
import com.example.gurmedefteri.R
import com.example.gurmedefteri.data.entity.NewUser
import com.example.gurmedefteri.databinding.FragmentSigninScreenBinding
import com.example.gurmedefteri.ui.viewmodels.SigninScreenViewModel
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import dagger.hilt.android.AndroidEntryPoint

@AndroidEntryPoint
class SigninScreenFragment : Fragment() {
    private lateinit var binding : FragmentSigninScreenBinding
    private lateinit var  viewModel: SigninScreenViewModel

    private lateinit var signInPass: EditText
    private var isPasswordVisible: Boolean = false

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentSigninScreenBinding.inflate(inflater, container, false)
        signInPass = binding.signInPass

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
        binding.textViewLogin.setOnClickListener {
            activity?.onBackPressedDispatcher?.onBackPressed()
        }
        binding.button4.setOnClickListener {
            var everythingOk = true

            val name = binding.signInName.text.toString()
            val email = binding.signInEmail.text.toString()
            val ageText = binding.signInAge.text.toString()
            var age = -5

            val pass = binding.signInPass.text.toString()




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
            val regexOnlyLetter = Regex("^[a-zA-Z]+$")
            /*if(!(regexOnlyLetter.matches(name))&&everythingOk){
                everythingOk = false
                alert("İsim Hatası","İsim Sadece Harflerden Oluşmalıdır")
            }*/
            if(ageText !="" && everythingOk){
                age= ageText.toInt()
            }
            else{
                if(everythingOk){
                    alert("Yaş Hatası","Lütfen Yaşınızı Boş Bırakmayınız")
                    everythingOk =false
                }
            }

            if(age>0 && age>200 && everythingOk){
                everythingOk = false
                alert("Yaş Hatası","Yaşınız 200 den fazla olamaz")
            }
            val regexForPass = Regex("^(?=.*\\d)(?=.*[A-Z])(?=.*[a-z]).{8,100}$")
            if (!regexForPass.matches(pass) && everythingOk) {
                everythingOk = false
                alert("Parola Hatası", "Parola en az 8 karakterden oluşmalı ve içinde şunlar bulunmalıdır: büyük harf, küçük harf ve sayı")
            }
            if(everythingOk){
                val user = NewUser(name,email,pass,"User",age)
                viewModel.addUser(user)
            }

        }

        viewModel.succes.observe(viewLifecycleOwner){
            try {
                val success = it
                if(success){
                    val navController = findNavController()
                    navController.popBackStack()
                    navController.navigate(R.id.loginScreenFragment)
                }
                else{

                }

            }catch (e:Exception){

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
    fun alert(title:String, message:String){
        MaterialAlertDialogBuilder(requireContext())
            .setTitle(title)
            .setMessage(message)
            .setPositiveButton("Tamam "){d,i ->
            }
            .show()
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val tempViewModel: SigninScreenViewModel by viewModels()
        viewModel = tempViewModel
    }

}