package com.example.gurmedefteri.ui.fragments

import android.content.Intent
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.viewModels
import com.example.gurmedefteri.R
import com.example.gurmedefteri.SplashScreen
import com.example.gurmedefteri.databinding.FragmentLogoutBinding
import com.example.gurmedefteri.ui.viewmodels.LogoutViewModel
import com.example.gurmedefteri.ui.viewmodels.ProfileViewModel
import dagger.hilt.android.AndroidEntryPoint

@AndroidEntryPoint
class LogoutFragment : Fragment() {
    private lateinit var binding:FragmentLogoutBinding
    private lateinit var viewModel: LogoutViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentLogoutBinding.inflate(inflater, container, false)
        viewModel.logOut()
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
        return binding.root
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val tempViewModel: LogoutViewModel by viewModels()
        viewModel = tempViewModel
    }

}