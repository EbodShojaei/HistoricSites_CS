//
//  HistoricSite.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import Foundation

struct HistoricSite: Codable, Identifiable, Hashable {
    let id: Int
    var name: String
    var description: String
    var countries: String
    var latitude: Double
    var longitude: Double
    var imageBase64: String?
    var averageRating: Double
    var totalReviews: Int
    
    // Add Hashable conformance
    func hash(into hasher: inout Hasher) {
        hasher.combine(id)
    }
    
    static func == (lhs: HistoricSite, rhs: HistoricSite) -> Bool {
        lhs.id == rhs.id
    }
    
    enum CodingKeys: String, CodingKey {
        case id, name, description, countries, latitude, longitude, imageBase64, averageRating, totalReviews
    }
    
    // Add the update method to create new instances with modified values
    func updated(
        name: String? = nil,
        description: String? = nil,
        countries: String? = nil,
        latitude: Double? = nil,
        longitude: Double? = nil,
        imageBase64: String? = nil,
        averageRating: Double? = nil,
        totalReviews: Int? = nil
    ) -> HistoricSite {
        return HistoricSite(
            id: self.id,
            name: name ?? self.name,
            description: description ?? self.description,
            countries: countries ?? self.countries,
            latitude: latitude ?? self.latitude,
            longitude: longitude ?? self.longitude,
            imageBase64: imageBase64 ?? self.imageBase64,
            averageRating: averageRating ?? self.averageRating,
            totalReviews: totalReviews ?? self.totalReviews
        )
    }
}

// API Response structure
struct APIResponse<T: Codable>: Codable {
    let success: Bool
    let count: Int?
    let data: T
    let message: String?
}
