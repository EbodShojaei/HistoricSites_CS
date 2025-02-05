//
//  MapView.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import SwiftUI
import MapKit

struct MapView: View {
    @EnvironmentObject private var viewModel: HistoricSitesViewModel
    @State private var camera: MapCameraPosition = .automatic
    @State private var selectedSite: HistoricSite?
    
    var body: some View {
        NavigationView {
            ZStack {
                Map(selection: $selectedSite) {
                    ForEach(viewModel.historicSites) { site in
                        Marker(site.name, coordinate: CLLocationCoordinate2D(
                            latitude: site.latitude,
                            longitude: site.longitude
                        ))
                        .tint(.red)
                        .tag(site)
                    }
                }
                .mapControls {
                    MapCompass()
                    MapScaleView()
                }
                
                if let site = selectedSite {
                    VStack {
                        Spacer()
                        NavigationLink(destination: HistoricSiteDetailView(site: site)) {
                            SitePreviewCard(site: site) {
                                selectedSite = nil
                            }
                        }
                        .buttonStyle(PlainButtonStyle())
                    }
                    .transition(.move(edge: .bottom))
                    .animation(.easeInOut, value: selectedSite)
                }
            }
            .navigationTitle("Map")
            .task {
                await viewModel.loadHistoricSites()
            }
        }
    }
}

struct SitePreviewCard: View {
    let site: HistoricSite
    let onDismiss: () -> Void
    
    var body: some View {
        VStack(alignment: .leading, spacing: 8) {
            HStack {
                if let uiImage = UIImage.fromBase64(site.imageBase64) {
                    Image(uiImage: uiImage)
                        .resizable()
                        .aspectRatio(contentMode: .fill)
                        .frame(width: 50, height: 50)
                        .clipShape(Circle())
                } else {
                    Circle()
                        .fill(Color.gray.opacity(0.3))
                        .frame(width: 50, height: 50)
                        .overlay(
                            Image(systemName: "photo")
                                .foregroundColor(.gray)
                        )
                }
                
                VStack(alignment: .leading) {
                    Text(site.name)
                        .font(.headline)
                    Text(site.countries)
                        .font(.subheadline)
                        .foregroundColor(.secondary)
                }
                
                Spacer()
                
                Button(action: onDismiss) {
                    Image(systemName: "xmark.circle.fill")
                        .foregroundColor(.gray)
                }
            }
            
            Text(site.description)
                .font(.caption)
                .lineLimit(2)
                .padding(.vertical, 4)
            
            Text("Tap for details")
                .frame(maxWidth: .infinity)
                .padding(.vertical, 8)
                .background(Color.blue)
                .foregroundColor(.white)
                .cornerRadius(8)
        }
        .padding()
        .background(Color(.systemBackground))
        .cornerRadius(12)
        .shadow(radius: 5)
        .padding()
    }
}
