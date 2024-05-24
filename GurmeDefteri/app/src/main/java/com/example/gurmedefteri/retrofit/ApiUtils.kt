package com.example.gurmedefteri.retrofit

class ApiUtils {
    companion object{
        val BASE_URL = "http://34.125.160.14:5000/"

        fun getApiService() : ApiService {
            return RetrofitClient.getRetrofit(BASE_URL).create(ApiService::class.java)
        }
    }
}