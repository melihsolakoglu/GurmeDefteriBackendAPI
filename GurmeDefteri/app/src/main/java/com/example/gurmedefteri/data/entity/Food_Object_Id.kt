package com.example.gurmedefteri.data.entity

import org.bson.types.ObjectId
import java.io.Serializable

class Food_Object_Id(
    val country: String = "",
    val name: String = "",
    val image: String = "",
    val id: Id = Id(),
    val category: String = ""
) : Serializable {
    data class Id(
        val timestamp: Int = (System.currentTimeMillis() / 1000).toInt(),
        val machine: Int = 0,
        val pid: Int = 0,
        val increment: Int = 0
    ) : Serializable {
        fun toObjectId(): String {
            val byteArray = ByteArray(12)
            byteArray[0] = (timestamp ushr 24).toByte()
            byteArray[1] = (timestamp ushr 16).toByte()
            byteArray[2] = (timestamp ushr 8).toByte()
            byteArray[3] = timestamp.toByte()
            byteArray[4] = (machine ushr 8).toByte()
            byteArray[5] = machine.toByte()
            byteArray[6] = (pid ushr 8).toByte()
            byteArray[7] = pid.toByte()
            byteArray[8] = (increment ushr 24).toByte()
            byteArray[9] = (increment ushr 16).toByte()
            byteArray[10] = (increment ushr 8).toByte()
            byteArray[11] = increment.toByte()

            return ObjectId(byteArray).toHexString()
        }
    }


}