package com.example.gurmedefteri.data.datastore

import android.content.Context
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.core.booleanPreferencesKey
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.emptyPreferences
import androidx.datastore.preferences.core.stringPreferencesKey
import androidx.datastore.preferences.preferencesDataStore
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.catch
import kotlinx.coroutines.flow.map
import java.io.IOException

class UserPreferences (context: Context){
    private val Context.dataStore: DataStore<Preferences> by preferencesDataStore(name = "USERINFO_KEY")
    private val dataStore = context.dataStore

    companion object {
        val LoggedIn = booleanPreferencesKey("LOGGED_CONTROL_KEY")
        val JWTToken = stringPreferencesKey("JWT_TOKEN_KEY")
        val Id = stringPreferencesKey("USER_ID")

        const val DEFAULT_LOGGED_IN = false
        const val DEFAULT_JWT_TOKEN = ""
        const val DEFAULT_USER_ID = ""
    }

    suspend fun setLoggedIn(loggedInBool: Boolean) {
        dataStore.edit { preferences ->
            preferences[LoggedIn] = loggedInBool
        }
    }

    suspend fun setJWTToken(jwtToken: String) {
        dataStore.edit { preferences ->
            preferences[JWTToken] = jwtToken
        }
    }

    suspend fun setUserId(id: String) {
        dataStore.edit { preferences ->
            preferences[Id] = id
        }
    }

    fun getLoggedIn(): Flow<Boolean> {
        return dataStore.data
            .catch { exception ->
                if (exception is IOException) {
                    emit(emptyPreferences())
                } else {
                    throw exception
                }
            }
            .map { preferences ->
                val uiMode = preferences[LoggedIn] ?: DEFAULT_LOGGED_IN
                uiMode
            }
    }

    fun getJWTToken(): Flow<String> {
        return dataStore.data
            .catch { exception ->
                if (exception is IOException) {
                    emit(emptyPreferences())
                } else {
                    throw exception
                }
            }
            .map { preferences ->
                val uiMode = preferences[JWTToken] ?: DEFAULT_JWT_TOKEN
                uiMode
            }
    }

    fun getUserId(): Flow<String> {
        return dataStore.data
            .catch { exception ->
                if (exception is IOException) {
                    emit(emptyPreferences())
                } else {
                    throw exception
                }
            }
            .map { preferences ->
                val uiMode = preferences[Id] ?: DEFAULT_USER_ID
                uiMode
            }
    }
    suspend fun clearPreferences() {
        dataStore.edit { preferences ->
            preferences.clear()
        }
    }


}