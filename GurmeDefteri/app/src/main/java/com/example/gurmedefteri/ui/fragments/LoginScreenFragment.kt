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
import androidx.navigation.Navigation
import androidx.navigation.fragment.findNavController
import com.example.gurmedefteri.MainActivity
import com.example.gurmedefteri.R
import com.example.gurmedefteri.databinding.FragmentLoginScreenBinding
import com.example.gurmedefteri.ui.viewmodels.LoginScreenViewModel
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import dagger.hilt.android.AndroidEntryPoint

@AndroidEntryPoint
class LoginScreenFragment : Fragment() {
    private lateinit var binding : FragmentLoginScreenBinding
    private lateinit var  viewModel: LoginScreenViewModel

    private lateinit var logInPass: EditText
    private var isPasswordVisible: Boolean = false

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentLoginScreenBinding.inflate(inflater, container, false)

        logInPass = binding.loginScreenPasswordTextview

        logInPass.setOnTouchListener { v, event ->
            val DRAWABLE_END = 2
            if (event.action == MotionEvent.ACTION_UP) {
                if (event.rawX >= (logInPass.right - logInPass.compoundDrawables[DRAWABLE_END].bounds.width())) {
                    togglePasswordVisibility()
                    return@setOnTouchListener true
                }
            }
            false
        }

        binding.LoginButton.setOnClickListener {
            val username = binding.loginScreenEmailTextview.text.toString()
            val password = binding.loginScreenPasswordTextview.text.toString()
            viewModel.loginControl(username, password)
        }
        binding.textViewRegister.setOnClickListener {
            val navController = findNavController()
            navController.navigate(R.id.action_loginScreenFragment_to_signinScreenFragment)
        }
        viewModel.loggedIn.observe(viewLifecycleOwner){
            try {
                val loggedIn = it
                if(loggedIn){
                    viewModel.setLoggedIn(loggedIn)
                }else{
                    alert("Giriş Başarısız","Hatalı Email Veya Password Girdiniz.")
                }



            }catch (e:Exception){

            }

        }
        viewModel.jwtToken.observe(viewLifecycleOwner){
            try {
                val jwtToken = it
                viewModel.setJWTToken(jwtToken)

            }catch (e:Exception){
                Log.d("ERROR",e.toString())
            }

        }
        viewModel.viewModelPassword.observe(viewLifecycleOwner){
            try {
                val pass = it
                viewModel.setUserPass(pass)

            }catch (e:Exception){
                Log.d("ERROR",e.toString())
            }

        }
        viewModel.id.observe(viewLifecycleOwner){
            try {
                Log.d("said1",it)
                val id = it
                viewModel.setUserId(id)

            }catch (e:Exception){
                Log.d("ERROR",e.toString())
            }

        }
        viewModel.Email.observe(viewLifecycleOwner){
            try {
                val email = it
                viewModel.setUserEmail(email)

            }catch (e:Exception){
                Log.d("ERROR",e.toString())
            }

        }
        viewModel.succes.observe(viewLifecycleOwner){
            try {
                val success = it
                if(success){
                    val intent = Intent(activity, MainActivity::class.java).apply {
                        flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                    }
                    startActivity(intent)
                }
                else{

                }

            }catch (e:Exception){
                Log.d("ERROR",e.toString())
            }

        }

        return binding.root
    }
    private fun togglePasswordVisibility() {
        if (isPasswordVisible) {
            // Şifreyi gizle
            logInPass.inputType = InputType.TYPE_CLASS_TEXT or InputType.TYPE_TEXT_VARIATION_PASSWORD
            logInPass.setCompoundDrawablesWithIntrinsicBounds(R.drawable.ic_lock_login, 0, R.drawable.password_eye_2, 0)
        } else {
            // Şifreyi göster
            logInPass.inputType = InputType.TYPE_CLASS_TEXT
            logInPass.setCompoundDrawablesWithIntrinsicBounds(R.drawable.ic_lock_login, 0, R.drawable.password_eye, 0)
        }
        // İmleç konumunu koru
        logInPass.setSelection(logInPass.text.length)
        isPasswordVisible = !isPasswordVisible
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val tempViewModel: LoginScreenViewModel by viewModels()
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
}