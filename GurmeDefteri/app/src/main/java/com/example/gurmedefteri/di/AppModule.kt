package com.example.gurmedefteri.di

import android.content.Context
import com.example.gurmedefteri.data.datasource.ApiServicesDataSource
import com.example.gurmedefteri.data.datastore.UserPreferences
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import com.example.gurmedefteri.retrofit.ApiUtils
import com.example.gurmedefteri.retrofit.ApiService
import com.example.gurmedefteri.ui.viewmodels.SearchFoodsViewModel
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.android.qualifiers.ApplicationContext
import dagger.hilt.components.SingletonComponent
import javax.inject.Singleton

@Module
@InstallIn(SingletonComponent::class)
class AppModule {
    
    @Provides
    @Singleton
    fun provideApiServicesRepository(apiServiceDS:ApiServicesDataSource) : ApiServicesRepository {
        return ApiServicesRepository(apiServiceDS)
    }

    @Provides
    @Singleton
    fun provideApiServicesDataSource(apiService:ApiService) : ApiServicesDataSource {
        return ApiServicesDataSource(apiService)
    }

    @Provides
    @Singleton
    fun provideApiService() : ApiService {
        return ApiUtils.getApiService()
    }

    @Provides
    @Singleton
    fun provideUserPreferences(@ApplicationContext context: Context): UserPreferences {
        return UserPreferences(context)
    }

}