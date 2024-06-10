package com.example.gurmedefteri.retrofit

import com.example.gurmedefteri.data.entity.AverageScoreRequest
import com.example.gurmedefteri.data.entity.AverageScoreResponse
import com.example.gurmedefteri.data.entity.LoginUser
import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface ScoreSuggestionApiService {

    @POST("api/yemeknumarasi")
    suspend fun getAverageScoreFood(@Body request: AverageScoreRequest) : Response<AverageScoreResponse>
}