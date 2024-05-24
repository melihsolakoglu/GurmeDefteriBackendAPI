package com.example.gurmedefteri.ui.viewmodels

import android.util.Log
import androidx.lifecycle.Lifecycle
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModel
import androidx.lifecycle.asLiveData
import androidx.lifecycle.viewModelScope
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.ResponseBody
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class SplashScreenViewModel@Inject constructor(
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel(){
    val getLoggedIn= userPreferences.getLoggedIn().asLiveData(Dispatchers.IO)
    val getJWTToken= userPreferences.getJWTToken().asLiveData(Dispatchers.IO)

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

    fun loginControl(username:String,password:String){
        CoroutineScope(Dispatchers.Main).launch {

            val response: Response<Any> = krepo.loginControl(username , password)
            response
            if (response.isSuccessful) {
                // HTTP isteği başarılı oldu
                Log.d("said","sonunda")
                val responseBody = response.body().toString()
                Log.d("said",responseBody)
                setLoggedIn(true)
                setJWTToken(responseBody)
            } else {
                val errorCode = response.code()
                Log.d("ERROR",errorCode.toString())
            }
        }
    }
}