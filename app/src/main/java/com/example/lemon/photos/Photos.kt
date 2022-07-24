package com.example.lemon.photos

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.ViewModel
import androidx.recyclerview.widget.GridLayoutManager
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.lemon.R


class Photos : Fragment() {

    companion object {
        fun newInstance() = Photos()
    }

    private lateinit var viewModel: PhotosViewModel
    private var photosRecyclerViewAdapter: PhotosRecyclerViewAdapter? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        viewModel = ViewModelProvider(this, object : ViewModelProvider.Factory {
            override fun <T : ViewModel?> create(modelClass: Class<T>): T {
                return PhotosViewModel() as T
            }
        }).get(PhotosViewModel::class.java)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_photos, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        val recycler = view.findViewById<RecyclerView>(R.id.PhotosRecyclerView)
        recycler.layoutManager = GridLayoutManager(view.context, 2)
        val photosRecyclerViewAdapter = PhotosRecyclerViewAdapter(viewModel.getItems())
        recycler.adapter = photosRecyclerViewAdapter
    }

}