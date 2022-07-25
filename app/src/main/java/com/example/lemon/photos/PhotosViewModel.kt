package com.example.lemon.photos

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.lemon.Photo
import kotlin.random.Random


class PhotosViewModel : ViewModel() {

    private val mutablePhotosCollection: MutableLiveData<List<Photo>> = MutableLiveData()

    val photosCollection: LiveData<List<Photo>> = mutablePhotosCollection

    init {
        mutablePhotosCollection.postValue(generateItems())
    }

    private fun generateItems(): List<Photo> {
        return MutableList(Random.nextInt(25)) {
            Photo(
                makeName(Random.nextInt(5, 15)),
                makeName(25),
                0
            )
        }
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