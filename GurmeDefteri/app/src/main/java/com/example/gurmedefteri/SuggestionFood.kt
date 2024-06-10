package com.example.gurmedefteri

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.example.gurmedefteri.R
import com.example.gurmedefteri.databinding.ActivityLoginBinding
import com.example.gurmedefteri.databinding.ActivitySplashScreenBinding
import com.example.gurmedefteri.databinding.ActivitySuggestionFoodBinding
import dagger.hilt.android.AndroidEntryPoint

@AndroidEntryPoint
class SuggestionFood : AppCompatActivity() {
    private lateinit var binding: ActivitySuggestionFoodBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivitySuggestionFoodBinding.inflate(layoutInflater)
        setContentView(binding.root)
    }
}