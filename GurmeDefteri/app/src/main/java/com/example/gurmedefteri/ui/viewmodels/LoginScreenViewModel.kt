package com.example.gurmedefteri.ui.viewmodels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.User
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class LoginScreenViewModel @Inject constructor (
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel(){
    val succes = MutableLiveData<Boolean>()
    val loggedIn = MutableLiveData<Boolean>()
    val jwtToken = MutableLiveData<String>()
    val id = MutableLiveData<String>()

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
    fun setUserEmail(id : String) {
        viewModelScope.launch(Dispatchers.IO) {
            userPreferences.setUserId(id)
        }
    }
    fun getUserByMail(userEmail:String){
        CoroutineScope(Dispatchers.Main).launch {

            try {
                val response: Response<User> = krepo.getUserByMail(userEmail)
                if (response.isSuccessful) {
                    val anan = response.body()?.id
                    if(anan != null){
                        id.value = anan.toString()
                        succes.value = true
                    }
                } else {
                    val errorCode = response.code()
                    Log.d("Error",errorCode.toString())
                }
            } catch (e: Exception) {
                Log.d("ERROR", "Request failed", e)
            }


        }
    }
    fun loginControl(username:String,password:String){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response: Response<Any> = krepo.loginControl(username , password)
                if (response.isSuccessful) {
                    val responseBody = response.body().toString()
                    jwtToken.value = responseBody
                    loggedIn.value = true
                    getUserByMail(username)

                } else {
                    loggedIn.value = false
                    val errorCode = response.code()
                    Log.d("Error",errorCode.toString())
                }
            } catch (e: Exception) {
                Log.d("ERROR", "Request failed", e)
            }
        }
    }
}