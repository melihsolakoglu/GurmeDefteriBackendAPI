package com.example.gurmedefteri.ui.viewmodels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.cachedIn
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import com.example.gurmedefteri.ui.adapters.pagination.CategorizeUnscoredPaginationSource
import com.example.gurmedefteri.ui.adapters.pagination.MoreFoodsSource
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class MoreFoodsViewModel @Inject constructor(
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel() {
    val userId = MutableLiveData<String>()
    val scoreOrUnscoreViewModel = MutableLiveData<Int>()
    val queryKeyViewModel = MutableLiveData<String>()

    val mainFood = MutableLiveData<Boolean>()

    init {
        viewModelScope.launch {
            userPreferences.getUserId().collect { id ->
                userId.value = id
            }
        }
    }
    fun controlMainFoodsLists(){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                var response:Response<List<Food>>
                if (scoreOrUnscoreViewModel.value == 1){
                    response = krepo.getUnscoredCategorizeFoodsByPageWithUserId(userId.value!!,queryKeyViewModel.value!!,1,6)
                } else if(scoreOrUnscoreViewModel.value == 2){
                    response = krepo.getScoredCategorizeFoodsByPageWithUserId(userId.value!!,queryKeyViewModel.value!!,1,6)
                }else{
                    response = krepo.getUnscoredCategorizeFoodsByPageWithUserId(userId.value!!,queryKeyViewModel.value!!,1,6)

                }
                if(response.isSuccessful){
                    mainFood.value =true


                }else{
                    mainFood.value = false

                }

            } catch (e: Exception) {
                mainFood.value = false
            }
        }

    }

    var foodsList = Pager(PagingConfig(1)) {
        MoreFoodsSource(userId.value.toString(),scoreOrUnscoreViewModel.value!!, queryKeyViewModel.value!!,krepo)
    }.flow.cachedIn(viewModelScope)

    fun changeLists(){
        foodsList = Pager(PagingConfig(1)){
            MoreFoodsSource(userId.value.toString(),scoreOrUnscoreViewModel.value!!, queryKeyViewModel.value!!,krepo)
        }.flow.cachedIn(viewModelScope)

    }


}