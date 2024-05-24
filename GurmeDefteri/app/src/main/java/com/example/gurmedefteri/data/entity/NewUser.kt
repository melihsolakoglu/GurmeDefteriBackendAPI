package com.example.gurmedefteri.data.entity

import java.io.Serializable

data class NewUser(val Name: String = "",
                   val Email: String = "",
                   val Password: String = "",
                   val Role: String= "",
                   val Age: Int): Serializable {
}