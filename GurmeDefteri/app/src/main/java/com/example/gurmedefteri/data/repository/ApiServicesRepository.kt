package com.example.gurmedefteri.data.repository

import com.example.gurmedefteri.data.datasource.ApiServicesDataSource
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.entity.NewUser
import com.example.gurmedefteri.data.entity.ScoredFoods
import com.example.gurmedefteri.data.entity.User
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import okhttp3.ResponseBody
import retrofit2.Response


class ApiServicesRepository(var kds:ApiServicesDataSource) {
    suspend fun loginControl(username:String , pass:String) : Response<Any> = kds.loginControl(username,pass)
    suspend fun addUser(user:NewUser) : Response<ResponseBody> = kds.addUser(user)
    suspend fun addScoredFoods(scoredFoods: ScoredFoods) : Response<ResponseBody> = kds.addScoredFoods(scoredFoods)
    suspend fun updateScoredFoods(userId: String, foodId: String, score: Int) : Response<ResponseBody> = kds.updateScoredFoods(userId, foodId, score)
    suspend fun getUserByMail(userMail: String) : Response<User> = kds.getUserByMail(userMail)
    suspend fun getUserById(userId: String) : Response<User> = kds.getUserById(userId)
    suspend fun updateUser(user:User) : Response<ResponseBody> = kds.updateUser(user)
    suspend fun getAllFoods() : Response<List<Food>> = kds.getAllFoods()
    suspend fun getFoodByName(foodName:String) : Response<Food> = kds.getFoodByName(foodName)
    suspend fun getFoodsByPageWithUserId(userId:String, page:Int, pageSize:Int) : Response<List<Food>> = kds.getFoodsByPageWithUserId(userId,page,pageSize)
    suspend fun getFoodSearchByPage(query:String, page:Int, pageSize:Int) : Response<List<Food>> = kds.getFoodSearchByPage(query ,page, pageSize)
    suspend fun getScoredFoodsByUserId(userId:String, page:Int, pageSize:Int) : Response<List<Food>> = kds.getScoredFoodsByUserId(userId ,page, pageSize)
    suspend fun checkScoredFood(userId:String, foodId:String) : Response<ResponseBody> = kds.checkScoredFood(userId ,foodId)
    suspend fun deleteUser(userId:String):Response<ResponseBody> = kds.deleteUser(userId)


    /*suspend fun kaydet(kisi_ad:String,kisi_tel:String) = kds.kaydet(kisi_ad, kisi_tel)

    suspend fun guncelle(kisi_id:Int,kisi_ad:String,kisi_tel:String) = kds.guncelle(kisi_id, kisi_ad, kisi_tel)

    suspend fun sil(kisi_id:Int) = kds.sil(kisi_id)

    suspend fun kisileriYukle() : List<Kisiler> = kds.kisileriYukle()

    suspend fun ara(aramaKelimesi:String) : List<Kisiler> = kds.ara(aramaKelimesi)*/
}