package com.example.gurmedefteri.ui.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class LogoutViewModel @Inject constructor (
    private val userPreferences: UserPreferences,
) : ViewModel(){

    val loggedOut = MutableLiveData<Boolean>()


    fun logOut(){
        viewModelScope.launch(Dispatchers.IO) {
            userPreferences.clearPreferences()
        }
        loggedOut.value = true
    }
}