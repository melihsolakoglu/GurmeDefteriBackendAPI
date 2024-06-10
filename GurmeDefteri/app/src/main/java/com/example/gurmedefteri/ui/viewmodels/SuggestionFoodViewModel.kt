package com.example.gurmedefteri.ui.viewmodels

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.util.Base64
import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.entity.ScoredFoods
import com.example.gurmedefteri.data.entity.SuggestionFood
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.ResponseBody
import retrofit2.Response
import javax.inject.Inject

@HiltViewModel
class SuggestionFoodViewModel @Inject constructor(
    private val userPreferences: UserPreferences,
    private val krepo: ApiServicesRepository
) : ViewModel(){

    val userId = MutableLiveData<String>()
    val getSuggestionFood = MutableLiveData<SuggestionFood>()
    val destroySuggestionScore = MutableLiveData<Boolean>()

    var succes = MutableLiveData<Boolean>()
    var scoreViewModel = MutableLiveData<Int>().apply { value = 0 }

    init {
        viewModelScope.launch {
            userPreferences.getUserId().collect { id ->
                userId.value = id
            }
        }

    }

    fun getFoodScoreSuggestion(userId: String?){
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val response: Response<SuggestionFood> = krepo.getFoodScoreSuggestion(userId.toString())
                if (response.isSuccessful) {
                    val responseBody = response.body()
                    getSuggestionFood.value = responseBody!!
                } else {
                    val errorBody = response.errorBody()?.string()
                }

            }catch (e:Exception){

                Log.d("ERROR", "Request failed", e)
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