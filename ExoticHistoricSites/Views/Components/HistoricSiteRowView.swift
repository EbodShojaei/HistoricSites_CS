//
//  HistoricSiteRowView.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import SwiftUI

struct HistoricSiteRowView: View {
    let site: HistoricSite
    
    var body: some View {
        HStack(spacing: 12) {
            if let uiImage = UIImage.fromBase64(site.imageBase64) {
                Image(uiImage: uiImage)
                    .resizable()
                    .aspectRatio(contentMode: .fill)
                    .frame(width: 60, height: 60)
                    .clipShape(Circle())
                    .shadow(radius: 2)
            } else {
                Circle()
                    .fill(Color.gray.opacity(0.3))
                    .frame(width: 60, height: 60)
                    .overlay(
                        Image(systemName: "photo")
                            .foregroundColor(.gray)
                    )
            }
            
            VStack(alignment: .leading, spacing: 4) {
                Text(site.name)
                    .font(.headline)
                
                Text(site.countries)
                    .font(.subheadline)
                    .foregroundColor(.secondary)
                
                HStack {
                    Image(systemName: "star.fill")
                        .foregroundColor(.yellow)
                    Text(String(format: "%.1f", site.averageRating))
                        .foregroundColor(.secondary)
                    Text("(\(site.totalReviews))")
                        .foregroundColor(.secondary)
                }
                .font(.caption)
            }
            
            Spacer()
        }
        .padding(.vertical, 8)
    }
}
