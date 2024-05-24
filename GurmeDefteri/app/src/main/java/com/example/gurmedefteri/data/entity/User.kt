package com.example.gurmedefteri.data.entity

import java.io.Serializable

data class User (val name: String = "",
                 val email: String = "",
                 val password: String = "",
                 val id: String = "",
                 val role: String= "",
                 val age: Int): Serializable {
}