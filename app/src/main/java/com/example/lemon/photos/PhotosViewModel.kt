package com.example.lemon.photos

import android.R
import android.graphics.BitmapFactory
import androidx.lifecycle.ViewModel
import com.example.lemon.Photo
import kotlin.random.Random


class PhotosViewModel : ViewModel() {
    fun getItems(): List<Photo> {
        return MutableList(Random.nextInt(25)) { Photo(makeName(Random.nextInt(15)), "tjrtd", 0) }
    }

    private fun makeName(length: Int): String {
        var alphabet = "abcdefghijklmnopqrstuvwxyz";
        var name = ""
        for (i in 0..length) {
            name += alphabet[Random.nextInt(0, alphabet.length)]
        }

        return name
    }
}