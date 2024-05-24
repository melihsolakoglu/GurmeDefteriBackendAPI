package com.example.gurmedefteri

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.activity.viewModels
import com.example.gurmedefteri.databinding.ActivityLoginBinding
import com.example.gurmedefteri.databinding.ActivityMainBinding
import com.example.gurmedefteri.ui.viewmodels.LoginViewModel
import com.example.gurmedefteri.ui.viewmodels.SplashScreenViewModel
import dagger.hilt.android.AndroidEntryPoint

@AndroidEntryPoint
class Login : AppCompatActivity() {
    private lateinit var binding: ActivityLoginBinding
    private val viewModel: LoginViewModel by viewModels()
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityLoginBinding.inflate(layoutInflater)

        setContentView(binding.root)
    }
}