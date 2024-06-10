package com.example.gurmedefteri.ui.adapters

import android.annotation.SuppressLint
import android.content.Context
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.util.Base64
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.paging.PagingDataAdapter
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.RecyclerView
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.databinding.FoodsCardsDesignBinding
import javax.inject.Inject

class MoreFoodsAdapter @Inject() constructor () :
    PagingDataAdapter<Food, MoreFoodsAdapter.ViewHolder>(differCallback) {

    private lateinit var binding: FoodsCardsDesignBinding
    private lateinit var context: Context

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val inflater = LayoutInflater.from(parent.context)
        binding = FoodsCardsDesignBinding.inflate(inflater, parent, false)
        context = parent.context
        return ViewHolder()
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(getItem(position)!!)
        holder.setIsRecyclable(false)
        val layoutParams = holder.itemView.layoutParams as ViewGroup.MarginLayoutParams
        layoutParams.marginStart = 0 // dp yerine px değeri
        layoutParams.marginEnd = 0
        holder.itemView.layoutParams = layoutParams
    }
    inner class ViewHolder : RecyclerView.ViewHolder(binding.root) {

        @SuppressLint("SetTextI18n")
        fun bind(item: Food) {
            binding.apply {
                textViewFoodName.text = item.name
                val bitmap = base64ToBitmap(item.imageBytes)
                val desiredWidth = 470
                val desiredHeight = 370
                val resizedBitmap = Bitmap.createScaledBitmap(bitmap, desiredWidth, desiredHeight, true)
                imageView2.setImageBitmap(resizedBitmap)
                root.setOnClickListener {

                    onItemClickListener?.let {
                        it(item)
                    }
                }
            }
        }
    }
    fun base64ToBitmap(base64String: String): Bitmap {
        val decodedBytes = Base64.decode(base64String, Base64.DEFAULT)
        return BitmapFactory.decodeByteArray(decodedBytes, 0, decodedBytes.size)
    }
    private var onItemClickListener: ((Food) -> Unit)? = null

    fun setOnItemClickListener(listener: (Food) -> Unit) {
        onItemClickListener = listener

    }
    companion object {
        val differCallback = object : DiffUtil.ItemCallback<Food>() {
            override fun areItemsTheSame(oldItem: Food, newItem: Food): Boolean {
                return oldItem.id == oldItem.id
            }

            override fun areContentsTheSame(oldItem: Food, newItem: Food): Boolean {
                return oldItem == newItem
            }
        }
    }


}