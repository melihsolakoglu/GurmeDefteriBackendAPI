package com.example.gurmedefteri.ui.viewmodels

import android.util.Log
import androidx.lifecycle.Lifecycle
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModel
import androidx.lifecycle.asLiveData
import androidx.lifecycle.viewModelScope
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.launch
import okhttp3.ResponseBody
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class SplashScreenViewModel@Inject constructor(
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel(){
    val userId = MutableLiveData<String>()
    val userEmail = MutableLiveData<String>()
    val userPass = MutableLiveData<String>()
    val getLoggedIn = userPreferences.getLoggedIn().asLiveData(Dispatchers.IO)
    val getUserId = userPreferences.getUserId().asLiveData(Dispatchers.IO)
    val getUserPass = userPreferences.getUserPass().asLiveData(Dispatchers.IO)

    val loggedOk = MutableLiveData<Boolean>()

    init {
        viewModelScope.launch {
            userPreferences.getUserEmail().collect { email ->
                userEmail.value = email
            }

        }
        viewModelScope.launch {
            userPreferences.getUserPass().collect{ pass ->

                userPass.value = pass
            }
        }

    }

    fun setLoggedIn(loggedIn : Boolean) {
        viewModelScope.launch(Dispatchers.IO) {
            userPreferences.setLoggedIn(loggedIn)
        }
    }
    fun setJWTToken(jwtToken : String) {
        viewModelScope.launch(Dispatchers.IO) {
            userPreferences.setJWTToken(jwtToken)
        }
    }

    fun loginControl(){
        CoroutineScope(Dispatchers.Main).launch {
            val username = userEmail.value.toString()
            val password = userPass.value.toString()
            val response: Response<Any> = krepo.loginControl(username , password)
            if (response.isSuccessful) {
                val responseBody = response.body().toString()
                setLoggedIn(true)
                setJWTToken(responseBody)
                loggedOk.value = true
            } else {
                val errorCode = response.code()
                Log.d("ERROR",errorCode.toString())
                loggedOk.value = false
            }
        }
    }


}