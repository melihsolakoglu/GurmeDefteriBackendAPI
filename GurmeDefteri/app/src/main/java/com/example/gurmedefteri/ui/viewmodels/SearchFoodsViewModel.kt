package com.example.gurmedefteri.ui.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.cachedIn
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import com.example.gurmedefteri.ui.adapters.pagination.MainPaginationSource
import com.example.gurmedefteri.ui.adapters.pagination.SearchFoodsPagination
import dagger.hilt.android.lifecycle.HiltViewModel
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class SearchFoodsViewModel @Inject constructor (var krepo: ApiServicesRepository) : ViewModel(){

    var userControl = MutableLiveData<Response<Any>>()

    val isOnFoodSearch = MutableLiveData<Boolean>()

    val query = MutableLiveData<String>()

    val foodsList = MutableLiveData<Pager<Int,Food>>()



    var foodsssList = Pager(PagingConfig(1)){
        SearchFoodsPagination(krepo,query.value.toString())
    }.flow.cachedIn(viewModelScope)



}