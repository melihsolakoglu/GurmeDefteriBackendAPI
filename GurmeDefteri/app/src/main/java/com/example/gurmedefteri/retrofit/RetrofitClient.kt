package com.example.gurmedefteri.retrofit

import com.google.gson.GsonBuilder
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.concurrent.TimeUnit

class RetrofitClient {
    companion object {
        fun getRetrofit(baseUrl:String, timeoutInSeconds: Long) : Retrofit {
            val client = OkHttpClient.Builder()
                .readTimeout(timeoutInSeconds, TimeUnit.SECONDS) // Okuma zaman aşımı
                .writeTimeout(timeoutInSeconds, TimeUnit.SECONDS) // Yazma zaman aşımı
                .connectTimeout(timeoutInSeconds, TimeUnit.SECONDS) // Bağlantı zaman aşımı
                .callTimeout(timeoutInSeconds, TimeUnit.SECONDS) // Çağrı zaman aşımı
                .build()

            return Retrofit.Builder()
                .baseUrl(baseUrl)
                .client(client)
                .addConverterFactory(GsonConverterFactory.create(GsonBuilder().setLenient().create()))
                .build()
        }
    }
}