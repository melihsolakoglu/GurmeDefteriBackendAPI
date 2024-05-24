package com.example.gurmedefteri.ui.viewmodels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.asLiveData
import androidx.lifecycle.viewModelScope
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.User
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class MainViewModel @Inject constructor(
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel() {
    val getUserId= userPreferences.getUserId().asLiveData(Dispatchers.IO)
    val user = MutableLiveData<User?>()
    val loggedOut = MutableLiveData<Boolean>()

    fun getUserById(userId:String){
        CoroutineScope(Dispatchers.Main).launch {

            try {
                Log.d("said","mal")
                val response: Response<User> = krepo.getUserById(userId)
                if (response.isSuccessful) {
                    val anan = response?.body()
                        Log.d("said","salak")
                        user.value = anan
                        Log.d("said","geri")
                } else {
                    val errorCode = response.code()
                    Log.d("Error",errorCode.toString())
                }
            } catch (e: Exception) {
                Log.d("ERROR", "Request failed", e)
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