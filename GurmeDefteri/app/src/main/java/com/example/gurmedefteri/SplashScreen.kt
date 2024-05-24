package com.example.gurmedefteri

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.Handler
import android.util.Log
import android.view.animation.AnimationUtils
import androidx.activity.viewModels
import androidx.lifecycle.Observer
import androidx.navigation.NavController
import androidx.navigation.NavOptions
import androidx.navigation.fragment.NavHostFragment
import com.example.gurmedefteri.databinding.ActivitySplashScreenBinding
import com.example.gurmedefteri.ui.viewmodels.SplashScreenViewModel
import dagger.hilt.android.AndroidEntryPoint
import android.content.Context
import android.content.DialogInterface
import android.net.ConnectivityManager
import android.os.Looper
import android.view.View
import android.widget.TextView
import androidx.appcompat.app.AlertDialog
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch

@AndroidEntryPoint
class SplashScreen : AppCompatActivity() {
    private lateinit var binding: ActivitySplashScreenBinding
    private val viewModel: SplashScreenViewModel by viewModels()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivitySplashScreenBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val topAnimation = AnimationUtils.loadAnimation(this,R.anim.top_animation)
        val middleAnimation = AnimationUtils.loadAnimation(this,R.anim.middle_animation)
        val bottomAnimation = AnimationUtils.loadAnimation(this,R.anim.bottom_animation)

        binding.imageView5.startAnimation(topAnimation)
        binding.MiddleTextView.startAnimation(middleAnimation)
        binding.BottomTextView.startAnimation(bottomAnimation)



        val splashScreenTimeOut = 4000
        val homeIntent = Intent(this@SplashScreen, MainActivity::class.java)
        val loginIntent = Intent(this@SplashScreen, Login::class.java)

        Handler().postDelayed({
            if (!isInternetAvailable()) {
                showNoInternetDialog()
            } else {
                viewModel.getLoggedIn.observe(this, Observer { loggedIn ->
                    Log.d("said", loggedIn.toString())
                    if (loggedIn) {
                        startActivity(homeIntent)
                    } else {
                        startActivity(loginIntent)
                    }
                    finish()
                })
            }
        }, splashScreenTimeOut.toLong())

    }

    private fun isInternetAvailable(): Boolean {
        val connectivityManager = getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager
        val networkInfo = connectivityManager.activeNetworkInfo
        return networkInfo != null && networkInfo.isConnected
    }

    private fun showNoInternetDialog() {
        val builder = AlertDialog.Builder(this)
        builder.setTitle("No Internet Connection")
        builder.setMessage("Siktir Git İnternetini Bağla Mk Fakiri.")
        builder.setPositiveButton("OK") { dialog: DialogInterface, _: Int ->
            dialog.dismiss()
            finish()
        }
        val dialog = builder.create()
        dialog.show()
    }
}