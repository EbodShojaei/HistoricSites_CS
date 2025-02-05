//
//  HistoricSiteDetailView.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import SwiftUI
import MapKit

struct HistoricSiteDetailView: View {
    @EnvironmentObject private var viewModel: HistoricSitesViewModel
    let site: HistoricSite
    @State private var showingImagePicker = false
    @State private var selectedImage: UIImage?
    @State private var isEditing = false
    
    var body: some View {
        ScrollView {
            VStack(alignment: .leading, spacing: 0) { // Remove spacing here
                // Image Section
                Group {
                    if let uiImage = UIImage.fromBase64(site.imageBase64) {
                        Image(uiImage: uiImage)
                            .resizable()
                            .aspectRatio(contentMode: .fill)
                            .frame(width: UIScreen.main.bounds.width, height: 300)
                            .clipped()
                    } else {
                        Rectangle()
                            .fill(Color.gray.opacity(0.3))
                            .frame(height: 300)
                            .overlay(
                                Image(systemName: "photo")
                                    .font(.largeTitle)
                                    .foregroundColor(.gray)
                            )
                    }
                }
                .onTapGesture {
                    showingImagePicker = true
                }
                
                // Info Section
                VStack(alignment: .leading, spacing: 12) {
                    Text(site.name)
                        .font(.title)
                        .fontWeight(.bold)
                    
                    Text(site.countries)
                        .font(.subheadline)
                        .foregroundColor(.secondary)
                    
                    Text(site.description)
                        .font(.body)
                        .padding(.top, 8)
                    
                    // Ratings Section
                    HStack {
                        Label("\(site.averageRating, specifier: "%.1f")", systemImage: "star.fill")
                            .foregroundColor(.yellow)
                        Text("(\(site.totalReviews) reviews)")
                            .foregroundColor(.secondary)
                    }
                    .padding(.top, 4)
                    
                    // Map Preview
                    Map(initialPosition: .region(MKCoordinateRegion(
                        center: CLLocationCoordinate2D(
                            latitude: site.latitude,
                            longitude: site.longitude
                        ),
                        span: MKCoordinateSpan(
                            latitudeDelta: 0.05,
                            longitudeDelta: 0.05
                        )
                    ))) {
                        Marker(site.name, coordinate: CLLocationCoordinate2D(
                            latitude: site.latitude,
                            longitude: site.longitude
                        ))
                        .tint(.red)
                    }
                    .mapStyle(.standard)
                    .frame(height: 200)
                    .cornerRadius(12)
                    .padding(.top, 8)
                }
                .padding()
            }
        }
        .navigationBarTitleDisplayMode(.inline)
        .sheet(isPresented: $showingImagePicker) {
            ImagePicker(image: $selectedImage)
        }
        .onChange(of: selectedImage) { _, newImage in
            if let image = newImage {
                Task {
                    await viewModel.uploadImage(for: site.id, uiImage: image)
                }
            }
        }
        .toolbar {
            ToolbarItem(placement: .navigationBarTrailing) {
                Button("Edit") {
                    isEditing.toggle()
                }
            }
        }
        .sheet(isPresented: $isEditing) {
            EditHistoricSiteView(site: site)
        }
    }
}
