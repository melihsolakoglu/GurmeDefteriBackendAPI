package com.example.gurmedefteri.retrofit

class ApiUtils {
    companion object{
        val BASE_URL = "http://34.125.160.14:5000/"
        val SCORE_SUGGESTION_BASE_URL = "http://20.81.205.102:92/"
        fun getApiService() : ApiService {
            return RetrofitClient.getRetrofit(BASE_URL,20).create(ApiService::class.java)
        }
        fun getScoreSuggestionApiServie(): ScoreSuggestionApiService{
            return RetrofitClient.getRetrofit(SCORE_SUGGESTION_BASE_URL,20).create(ScoreSuggestionApiService::class.java)
        }
    }
}