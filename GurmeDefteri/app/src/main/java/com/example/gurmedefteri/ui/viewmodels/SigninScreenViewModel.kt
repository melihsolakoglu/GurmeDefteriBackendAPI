package com.example.gurmedefteri.ui.viewmodels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.navigation.NavController
import androidx.navigation.Navigation
import com.example.gurmedefteri.R
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.NewUser
import com.example.gurmedefteri.data.entity.User
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.ResponseBody
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class SigninScreenViewModel @Inject constructor (
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel(){
    val succes = MutableLiveData<Boolean>()
    val isThereMail = MutableLiveData<String>()
    val addUser = MutableLiveData<Boolean>()
    fun addUser(user:NewUser){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response: Response<ResponseBody> = krepo.addUser(user)
                if (response.isSuccessful) {
                    val responseBody = response.body()?.string()
                    succes.value = true
                    Log.d("SUCCES",responseBody.toString())
                } else {
                    Log.e("API_ERROR", "Response not successful: ${response.errorBody()?.string()}")
                }
            } catch (e: Exception) {
                Log.d("ERROR", "Request failed", e)
            }
        }
    }


    fun getUserByMail(userEmail:String){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response: Response<User> = krepo.getUserByMail(userEmail)
                if (response.isSuccessful) {
                    isThereMail.value = "yes"

                } else {
                    isThereMail.value = "no"
                    val errorCode = response.code()
                    Log.d("Error",errorCode.toString())
                    Log.d("Error","2")
                }
            } catch (e: Exception) {
                Log.d("ERROR", "Request failed", e)
            }


        }
    }


}