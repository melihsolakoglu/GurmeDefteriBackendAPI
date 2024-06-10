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
import com.example.gurmedefteri.data.entity.AverageScoreRequest
import com.example.gurmedefteri.data.entity.AverageScoreResponse
import com.example.gurmedefteri.data.entity.Food
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

    var checkScoredFoodOk= MutableLiveData<Boolean>().apply { value = false }
    var getAverageScoreOk= MutableLiveData<Boolean>().apply { value = false }
    val AverageScoreFood = MutableLiveData<Int>()
    var isLoading= MutableLiveData<Boolean>()


    init {
        viewModelScope.launch {
            userPreferences.getUserId().collect { id ->
                userId.value = id
            }
        }

    }

    fun getAverageScoredFood(userId: String?, foodId: String){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val request = AverageScoreRequest(userId.toString(), foodId)
                val response: Response<ResponseBody> = krepo.getAverageScoredFood(request)
                val averageScoreResponse = response.body()
                if (response.isSuccessful) {
                    val averageScoreResponse = response.body()?.string()

                    val intValue = averageScoreResponse?.substringBefore(".")?.toIntOrNull()

                    if (intValue != null) {
                        AverageScoreFood.value = intValue!!
                        scored.value=false

                    } else {
                    }

                } else {
                    scored.value=true
                    val errorBody = response.errorBody()?.string()

                }

            }catch (e:Exception){

                Log.d("ERROR", "Request failed", e)
                scored.value=true
            }
            getAverageScoreOk.value =true
            isLoading.value =true

        }
    }


    fun checkScoredFood(userId: String?, foodId: String){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response: Response<ResponseBody> = krepo.checkScoredFood(userId.toString(),foodId)
                if (response.isSuccessful){
                    val responseBody = response.body()?.string()
                    scoreViewModel.value = responseBody?.toInt()

                    if(responseBody?.toInt() == 0){
                        scored.value = false
                    }else{
                        scored.value = true
                    }
                }else{

                    scored.value = false
                }

            }catch (e:Exception){

                scored.value = false
            }

            checkScoredFoodOk.value =true
            isLoading.value =true
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