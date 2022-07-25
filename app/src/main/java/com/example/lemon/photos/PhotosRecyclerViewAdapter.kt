package com.example.lemon.photos

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.ImageView
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.lemon.Photo
import com.example.lemon.R

class PhotosRecyclerViewAdapter(private var values: List<Photo>) :
    RecyclerView.Adapter<PhotosRecyclerViewAdapter.ViewHolder>() {

    class ViewHolder(itemView: View) :
        RecyclerView.ViewHolder(itemView) {


        val imageView: ImageView = itemView.findViewById(R.id.imageView)
        val buttonView: Button = itemView.findViewById(R.id.add_reaction)
        val userTextView: TextView = itemView.findViewById(R.id.photo_list_item_user)
        val countTextView: TextView = itemView.findViewById(R.id.photo_list_item_count)

//        override fun onClick(v: View?) {
//            val clicked = adapterPosition
//            listener.onListItemClick(clicked)
//        }

//        init {
//            itemView.setOnClickListener(this)
//        }
    }

    fun setItems(list: List<Photo>){
        values = list
        notifyDataSetChanged()
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val itemView = LayoutInflater.from(parent.context)
            .inflate(R.layout.photo_list_item, parent, false)
        return ViewHolder(itemView)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val photo = values[position]
        holder.userTextView.text = photo.userName
        holder.countTextView.text = photo.count.toString()
        holder.buttonView.setOnClickListener {
            photo.count++
            notifyItemChanged(position)
        }
    }

    override fun getItemCount(): Int = values.size;
}