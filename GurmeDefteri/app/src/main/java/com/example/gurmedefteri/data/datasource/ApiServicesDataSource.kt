package com.example.gurmedefteri.data.datasource

import com.example.gurmedefteri.data.entity.AverageScoreRequest
import com.example.gurmedefteri.data.entity.AverageScoreResponse
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.entity.LoginUser
import com.example.gurmedefteri.data.entity.NewUser
import com.example.gurmedefteri.data.entity.ScoredFoods
import com.example.gurmedefteri.data.entity.SuggestionFood
import com.example.gurmedefteri.data.entity.User
import com.example.gurmedefteri.retrofit.ApiService
import com.example.gurmedefteri.retrofit.ScoreSuggestionApiService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.http.Body

class ApiServicesDataSource(var kdao: ApiService, var sApiService:ScoreSuggestionApiService)  {
    suspend fun loginControl(email:String , pass:String) : Response<Any> =
        withContext(Dispatchers.IO){

            val user = LoginUser(email, pass)
            return@withContext kdao.girisControl(user)
        }

    suspend fun addUser(user:NewUser) : Response<ResponseBody> =
        withContext(Dispatchers.IO){

            return@withContext kdao.addUser(user)
        }
    suspend fun addScoredFoods(scoredFoods: ScoredFoods) : Response<ResponseBody> =
        withContext(Dispatchers.IO){

            return@withContext kdao.addScoredFoods(scoredFoods)
        }
    suspend fun updateScoredFoods(userId: String, foodId: String, score: Int) : Response<ResponseBody> =
        withContext(Dispatchers.IO){

            return@withContext kdao.updateScoredFoods(userId, foodId, score)
        }

    suspend fun getUserByMail(userMail: String) : Response<User> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getUserByMail(userMail)
        }
    suspend fun getUserById(userId: String) : Response<User> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getUserById(userId)
        }

    suspend fun getAllFoods() : Response<List<Food>> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getAllFoods()
        }

    suspend fun getFoodByName(foodName:String) : Response<Food> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getFoodByName(foodName)
        }
    suspend fun getFoodScoreSuggestion(userId:String) : Response<SuggestionFood> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getFoodScoreSuggestion(userId)
        }
    suspend fun getFoodsByPageWithUserId(userId: String,page:Int, pageSize:Int) : Response<List<Food>> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getFoodsByPageWithUserId(userId, page, pageSize)
        }

    suspend fun getUnscoredCategorizeFoodsByPageWithUserId(userId: String,queryKey:String,page:Int, pageSize:Int) : Response<List<Food>> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getUnscoredCategorizeFoodsByPageWithUserId(userId,queryKey, page, pageSize)
        }
    suspend fun getScoredCategorizeFoodsByPageWithUserId(userId: String,queryKey:String,page:Int, pageSize:Int) : Response<List<Food>> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getScoredCategorizeFoodsByPageWithUserId(userId,queryKey, page, pageSize)
        }
    suspend fun getFoodSearchByPage(query:String, page:Int, pageSize:Int) : Response<List<Food>> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getFoodSearchByPage(query ,page, pageSize)
        }
    suspend fun getScoredFoodsByUserId(userId:String, page:Int, pageSize:Int) : Response<List<Food>> =
        withContext(Dispatchers.IO){

            return@withContext kdao.getScoredFoodsByUserId(userId ,page, pageSize)
        }
    suspend fun checkScoredFood(userId:String,foodId:String) : Response<ResponseBody> =
        withContext(Dispatchers.IO){

            return@withContext kdao.checkScoredFood(userId ,foodId)
        }
    suspend fun updateUser(user:User): Response<ResponseBody> =
        withContext(Dispatchers.IO){
            return@withContext kdao.updateUser(user)
        }
    suspend fun deleteUser(userId:String): Response<ResponseBody> =
        withContext(Dispatchers.IO){
            return@withContext kdao.deleteUser(userId)
        }
    suspend fun getAverageScoredFood(request:AverageScoreRequest): Response<ResponseBody> =
        withContext(Dispatchers.IO){
            return@withContext kdao.getAverageScoreFood(request)
        }

    suspend fun checkUserIdExistsInScoredFoods(userId:String): Response<Boolean> =
        withContext(Dispatchers.IO){
            return@withContext kdao.checkUserIdExistsInScoredFoods(userId)
        }
}