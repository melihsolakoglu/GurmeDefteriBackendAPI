package com.example.gurmedefteri.data.entity

import org.bson.types.ObjectId
import java.io.Serializable

data class Food(
    val country: String = "",
    val name: String = "",
    val imageBytes: String = "",
    val id: String = "",
    val category: String = ""
) : Serializable