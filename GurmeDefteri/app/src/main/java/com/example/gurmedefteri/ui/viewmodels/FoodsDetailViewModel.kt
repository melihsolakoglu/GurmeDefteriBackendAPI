package com.example.gurmedefteri.ui.viewmodels

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.util.Base64
import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.asLiveData
import androidx.lifecycle.viewModelScope
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.NewUser
import com.example.gurmedefteri.data.entity.ScoredFoods
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.ResponseBody
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class FoodsDetailViewModel @Inject constructor(
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel(){
    val userId = MutableLiveData<String>()
    val getUserId = userPreferences.getUserId().asLiveData(Dispatchers.IO)
    var scoreViewModel = MutableLiveData<Int>().apply { value = 0 }
    var scored = MutableLiveData<Boolean>().apply { value = false }
    var succes = MutableLiveData<Boolean>()

    init {
        viewModelScope.launch {
            userPreferences.getUserId().collect { id ->
                userId.value = id
            }
        }

    }
    fun checkScoredFood(userId: String?, foodId: String){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response: Response<ResponseBody> = krepo.checkScoredFood(userId.toString(),foodId)
                if (response.isSuccessful){
                    val responseBody = response.body()?.string()
                    Log.d("said","aha geldi lah $responseBody")
                    scoreViewModel.value = responseBody?.toInt()
                    Log.d("said","çeviri ${scoreViewModel.value}")
                }else{

                }

            }catch (e:Exception){

            }
        }
    }



    fun addScoredFoods(foodId:String, score: Int){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val newScoredFoods = ScoredFoods(userId.value.toString() ,foodId,score)
                val response: Response<ResponseBody> = krepo.addScoredFoods(newScoredFoods)
                if (response.isSuccessful) {
                    val responseBody = response.body()?.string()
                    succes.value = true
                    scoreViewModel.value = score
                    Log.d("Said",responseBody.toString())
                } else {
                    Log.e("API_ERROR", "Response not successful: ${response.errorBody()?.string()}")
                }
            } catch (e: Exception) {
                Log.d("ERROR", "Request failed", e)
            }
        }
    }
    fun updateScoredFoods(foodId:String,score:Int){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response:Response<ResponseBody> = krepo.updateScoredFoods(userId.value.toString(), foodId, score)
                if(response.isSuccessful){
                    val responseBody = response.body()?.string()
                    Log.d("Said",responseBody.toString())
                }else{

                    Log.e("API_ERROR", "Response not successful: ${response.errorBody()?.string()}")
                }

            }catch (e:Exception){

                Log.d("ERROR", "Request failed", e)
            }
        }
    }
    fun base64ToBitmap(base64String: String): Bitmap {
        val decodedBytes = Base64.decode(base64String, Base64.DEFAULT)
        return BitmapFactory.decodeByteArray(decodedBytes, 0, decodedBytes.size)
    }
}