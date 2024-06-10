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
import androidx.paging.PagingSource
import androidx.paging.cachedIn
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.entity.FoodsList
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import com.example.gurmedefteri.ui.adapters.pagination.CategorizeUnscoredPaginationSource
import com.example.gurmedefteri.ui.adapters.pagination.MainPaginationSource
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.HttpException
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class HomepageViewModel @Inject constructor(
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel() {

    val userId = MutableLiveData<String>()

    val mainFood = MutableLiveData<Boolean>()
    val drinksFood = MutableLiveData<Boolean>()
    val desertsFood = MutableLiveData<Boolean>()
    val soapFood = MutableLiveData<Boolean>()

    val isLoading = MutableLiveData<Boolean>()
    val isScoredAnyFood = MutableLiveData<Boolean>()

    init {
        viewModelScope.launch {
            userPreferences.getUserId().collect { id ->
                userId.value = id
            }
        }
    }
    fun checkUserIdExistsInScoredFoods(){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response = krepo.checkUserIdExistsInScoredFoods(userId.value.toString())
                if(response.isSuccessful){
                    isScoredAnyFood.value = response.body()
                }else{
                    isScoredAnyFood.value = false
                }

            }catch (e:Exception){

            }
        }
    }

    fun controlMainFoodsLists(){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response = krepo.getUnscoredCategorizeFoodsByPageWithUserId(userId.value.toString(),"Ana Yemek",1, 1)
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
    fun controlSoapFoodsLists(){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response = krepo.getUnscoredCategorizeFoodsByPageWithUserId(userId.value.toString(),"Çorba",1, 1)
                if(response.isSuccessful){
                    soapFood.value = true

                }else{
                    soapFood.value = false
                }

            } catch (e: Exception) {
                soapFood.value = false
            }
        }

    }
    fun controlDesertsFoodsLists(){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response = krepo.getUnscoredCategorizeFoodsByPageWithUserId(userId.value.toString(),"Tatlı",1, 1)
                if(response.isSuccessful){
                    desertsFood.value = true

                }else{
                    desertsFood.value = false
                }

            } catch (e: Exception) {
                desertsFood.value = false
            }
        }

    }
    fun controlDrinksFoodsLists(){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response = krepo.getUnscoredCategorizeFoodsByPageWithUserId(userId.value.toString(),"İçicek",1, 1)
                if(response.isSuccessful){
                    drinksFood.value = true

                }else{
                    drinksFood.value = false
                }

            } catch (e: Exception) {
                drinksFood.value = false
            }
        }

    }

    var mainFoodsList = Pager(PagingConfig(1)){
        CategorizeUnscoredPaginationSource(userId.value.toString(),krepo,"Ana Yemek")
    }.flow.cachedIn(viewModelScope)

    var soapFoodsList = Pager(PagingConfig(1)){
        CategorizeUnscoredPaginationSource(userId.value.toString(),krepo,"Çorba")
    }.flow.cachedIn(viewModelScope)

    var desertFoodList = Pager(PagingConfig(1)){
        CategorizeUnscoredPaginationSource(userId.value.toString(),krepo,"Tatlı")
    }.flow.cachedIn(viewModelScope)

    var drinksFoodList = Pager(PagingConfig(1)){
        CategorizeUnscoredPaginationSource(userId.value.toString(),krepo,"İçicek")
    }.flow.cachedIn(viewModelScope)

    fun changeLists(){
        mainFoodsList = Pager(PagingConfig(1)){
            CategorizeUnscoredPaginationSource(userId.value.toString(),krepo,"Ana Yemek")
        }.flow.cachedIn(viewModelScope)

        soapFoodsList = Pager(PagingConfig(1)){
            CategorizeUnscoredPaginationSource(userId.value.toString(),krepo,"Çorba")
        }.flow.cachedIn(viewModelScope)

        desertFoodList = Pager(PagingConfig(1)){
            CategorizeUnscoredPaginationSource(userId.value.toString(),krepo,"Tatlı")
        }.flow.cachedIn(viewModelScope)

        drinksFoodList = Pager(PagingConfig(1)){
            CategorizeUnscoredPaginationSource(userId.value.toString(),krepo,"İçicek")
        }.flow.cachedIn(viewModelScope)
    }


}