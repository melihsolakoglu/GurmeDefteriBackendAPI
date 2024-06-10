package com.example.gurmedefteri.ui.viewmodels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.asLiveData
import androidx.lifecycle.viewModelScope
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.User
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import com.example.gurmedefteri.ui.fragments.LoginScreenFragmentDirections
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.ResponseBody
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class ProfileViewModel @Inject constructor (
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel(){

    val getUserId = userPreferences.getUserId().asLiveData(Dispatchers.IO)

    val loggedOut = MutableLiveData<Boolean>()
    val userId = MutableLiveData<String>()
    val user = MutableLiveData<User>()
    val updateSucces = MutableLiveData<Boolean>()
    val areYouSure = MutableLiveData<Boolean>()
    val isLoadingScreen = MutableLiveData<Boolean>()

    fun getUserById(userId:String){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response:Response<User> = krepo.getUserById(userId)

                val responseBody = response.body()
                user.value = responseBody!!
                isLoadingScreen.value = true

            }catch (e:Exception){

            }
        }
    }
    fun updateUser (name:String,email:String,age:Int){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val user = User(name,email,user.value?.password.toString(),user.value?.id.toString(),user.value?.role.toString(),age)
                val response: Response<ResponseBody> = krepo.updateUser(user)
                if (response.isSuccessful){
                    updateSucces.value = true
                    updateSucces.value = false
                }else{
                }
            }catch (e:Exception){

            }
        }
    }
    fun updateUser (name:String,email:String,pass:String,age:Int){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val user = User(name,email,pass,user.value?.id.toString(),user.value?.role.toString(),age)
                val response: Response<ResponseBody> = krepo.updateUser(user)
                if (response.isSuccessful){
                    updateSucces.value = true
                    updateSucces.value = false
                }else{

                }
            }catch (e:Exception){

            }
        }
    }
    fun deleteUser(){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response:Response<ResponseBody> = krepo.deleteUser(userId.value.toString())
                if(response.isSuccessful){
                    logOut()
                }else{
                }

            }catch (e:Exception){
            }
        }
    }

    fun logOut(){
        viewModelScope.launch(Dispatchers.IO) {
            userPreferences.clearPreferences()
        }
        loggedOut.value = true
    }

}