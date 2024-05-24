package com.example.gurmedefteri.retrofit

import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.entity.LoginUser
import com.example.gurmedefteri.data.entity.NewUser
import com.example.gurmedefteri.data.entity.ScoredFoods
import com.example.gurmedefteri.data.entity.User
import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path
import retrofit2.http.Query

interface ApiService {
    @POST("api/Auth")
    suspend fun girisControl(@Body logUser: LoginUser) : Response<Any>

    @GET("api/Admin/GetAllFoods")
    suspend fun getAllFoods() : Response<List<Food>>

    @GET("api/Admin/GetFoodByName")
    suspend fun getFoodByName(@Query("name") foodName: String) : Response<Food>

    @GET("api/User/GetScoredFoodsByUserId")
    suspend fun getScoredFoodsByUserId(@Query("userId")userId: String, @Query("page") page:Int, @Query("pageSize") pageSize:Int) : Response<List<Food>>

    @GET("api/User/GetUnscoredFoodsByUserId")
    suspend fun getFoodsByPageWithUserId(@Query("userId")userId:String,@Query("page") page:Int, @Query("pageSize") pageSize:Int) : Response<List<Food>>

    @GET("api/User/FoodSearch")
    suspend fun getFoodSearchByPage(@Query("query") query:String,@Query("page") page:Int, @Query("pageSize") pageSize:Int) : Response<List<Food>>

    @PUT("api/Admin/UpdateUser")
    suspend fun updateUser(@Body updatedUser: User): Response<ResponseBody>

    @POST("api/Admin/AddUser")
    suspend fun addUser(@Body logUser: NewUser) : Response<ResponseBody>

    @POST("api/Admin/AddScoredFoods")
    suspend fun addScoredFoods(@Body scoredFoods: ScoredFoods) : Response<ResponseBody>

    @PUT("api/User/UpdateScoredFood")
    suspend fun updateScoredFoods(@Query("userId") userId: String, @Query("foodId") foodId: String, @Query("score") score: Int) : Response<ResponseBody>

    @GET("api/Admin/GetUserByMail")
    suspend fun getUserByMail(@Query("userMail") userMail: String) : Response<User>

    @GET("api/Admin/GetUserById")
    suspend fun getUserById(@Query("userId") userId: String) : Response<User>

    @GET("api/User/CheckScoredFood")
    suspend fun checkScoredFood(@Query("userId") userId: String,@Query("foodId") foodId: String) : Response<ResponseBody>

    @DELETE("api/Admin/DeleteUser")
    suspend fun deleteUser(@Query("userId")userId: String) :Response<ResponseBody>

}