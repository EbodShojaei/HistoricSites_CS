//
//  APIService.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import Foundation

enum APIError: Error {
    case invalidURL
    case networkError(Error)
    case decodingError(Error)
    case serverError(String)
}

class APIService {
    static let shared = APIService()
    private let baseURL = Config.shared.apiBaseURL
    
    private init() {}
    
    // MARK: - GET Methods
    func fetchHistoricSites() async throws -> [HistoricSite] {
        guard let url = URL(string: "\(baseURL)/historicSites") else {
            throw APIError.invalidURL
        }
        
        let (data, _) = try await URLSession.shared.data(from: url)
        let response = try JSONDecoder().decode(APIResponse<[HistoricSite]>.self, from: data)
        return response.data
    }
    
    func fetchHistoricSite(id: Int) async throws -> HistoricSite {
        guard let url = URL(string: "\(baseURL)/historicSites/\(id)") else {
            throw APIError.invalidURL
        }
        
        let (data, _) = try await URLSession.shared.data(from: url)
        let response = try JSONDecoder().decode(APIResponse<HistoricSite>.self, from: data)
        return response.data
    }
    
    func searchHistoricSites(country: String? = nil, name: String? = nil) async throws -> [HistoricSite] {
        var urlComponents = URLComponents(string: "\(baseURL)/historicSites/search")
        var queryItems: [URLQueryItem] = []
        
        if let country = country {
            queryItems.append(URLQueryItem(name: "country", value: country))
        }
        if let name = name {
            queryItems.append(URLQueryItem(name: "name", value: name))
        }
        
        urlComponents?.queryItems = queryItems
        
        guard let url = urlComponents?.url else {
            throw APIError.invalidURL
        }
        
        let (data, _) = try await URLSession.shared.data(from: url)
        let response = try JSONDecoder().decode(APIResponse<[HistoricSite]>.self, from: data)
        return response.data
    }
    
    // MARK: - POST Methods
    func createHistoricSite(_ site: HistoricSite) async throws -> HistoricSite {
        guard let url = URL(string: "\(baseURL)/historicSites") else {
            throw APIError.invalidURL
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = try JSONEncoder().encode(site)
        
        let (data, _) = try await URLSession.shared.data(for: request)
        let response = try JSONDecoder().decode(APIResponse<HistoricSite>.self, from: data)
        return response.data
    }
    
    // MARK: - PUT Methods
    func updateHistoricSite(_ site: HistoricSite) async throws -> HistoricSite {
        guard let url = URL(string: "\(baseURL)/historicSites/\(site.id)") else {
            throw APIError.invalidURL
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "PUT"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = try JSONEncoder().encode(site)
        
        let (data, _) = try await URLSession.shared.data(for: request)
        let response = try JSONDecoder().decode(APIResponse<HistoricSite>.self, from: data)
        return response.data
    }
    
    // MARK: - DELETE Methods
    func deleteHistoricSite(id: Int) async throws {
        guard let url = URL(string: "\(baseURL)/historicSites/\(id)") else {
            throw APIError.invalidURL
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "DELETE"
        
        let (data, _) = try await URLSession.shared.data(for: request)
        if let jsonString = String(data: data, encoding: .utf8) {
            print("Delete response: \(jsonString)")
        }
    }
    
    // MARK: - Image Upload
    func uploadImage(siteId: Int, imageData: Data) async throws -> String {
        guard let url = URL(string: "\(baseURL)/historicSites/upload-image/\(siteId)") else {
            throw APIError.invalidURL
        }
        
        let boundary = UUID().uuidString
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("multipart/form-data; boundary=\(boundary)", forHTTPHeaderField: "Content-Type")
        
        var body = Data()
        body.append("--\(boundary)\r\n".data(using: .utf8)!)
        body.append("Content-Disposition: form-data; name=\"image\"; filename=\"image.jpg\"\r\n".data(using: .utf8)!)
        body.append("Content-Type: image/jpeg\r\n\r\n".data(using: .utf8)!)
        body.append(imageData)
        body.append("\r\n--\(boundary)--\r\n".data(using: .utf8)!)
        
        request.httpBody = body
        
        let (data, _) = try await URLSession.shared.data(for: request)
        let response = try JSONDecoder().decode(APIResponse<[String: String]>.self, from: data)
        return response.data["imageBase64"] ?? ""
    }
}
