package com.example.gurmedefteri.ui.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.asLiveData
import androidx.lifecycle.viewModelScope
import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.PagingData
import androidx.paging.cachedIn
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.entity.User
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import com.example.gurmedefteri.ui.adapters.pagination.MainPaginationSource
import com.example.gurmedefteri.ui.adapters.pagination.ScoredFoodsSource
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.launch
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class ScoredFoodViewModel @Inject constructor(
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
    val foodsList = Pager(PagingConfig(1)) {
        ScoredFoodsSource(userId.value.toString(),krepo)
    }.flow.cachedIn(viewModelScope)


}