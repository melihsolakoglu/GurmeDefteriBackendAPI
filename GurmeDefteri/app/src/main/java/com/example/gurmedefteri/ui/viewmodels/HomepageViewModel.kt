package com.example.gurmedefteri.ui.viewmodels

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.util.Base64
import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.cachedIn
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.entity.FoodsList
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import com.example.gurmedefteri.ui.adapters.pagination.MainPaginationSource
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class HomepageViewModel @Inject constructor(
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel() {

    val userId = MutableLiveData<String>()
    init {
        viewModelScope.launch {
            userPreferences.getUserId().collect { id ->
                userId.value = id
            }
        }
    }

    val foodsssList = Pager(PagingConfig(1)){
        MainPaginationSource(userId.value.toString(),krepo)
    }.flow.cachedIn(viewModelScope)



}