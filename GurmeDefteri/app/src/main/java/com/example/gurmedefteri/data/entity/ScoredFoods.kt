package com.example.gurmedefteri.data.entity

import java.io.Serializable

data class ScoredFoods (val userId: String = "",
                        val foodId: String = "",
                        val score: Int): Serializable {
}