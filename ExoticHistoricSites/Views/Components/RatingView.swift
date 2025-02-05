//
//  RatingView.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import SwiftUI

struct RatingView: View {
    let rating: Double
    let reviews: Int
    
    var body: some View {
        HStack(spacing: 4) {
            Image(systemName: "star.fill")
                .foregroundColor(.yellow)
            Text(String(format: "%.1f", rating))
                .fontWeight(.medium)
            Text("(\(reviews))")
                .foregroundColor(.secondary)
        }
    }
}
