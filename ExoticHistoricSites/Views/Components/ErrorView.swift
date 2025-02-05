//
//  ErrorView.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import SwiftUI

struct ErrorView: View {
    let message: String
    let retryAction: () -> Void
    
    var body: some View {
        VStack(spacing: 16) {
            Image(systemName: "exclamationmark.triangle")
                .font(.system(size: 50))
                .foregroundColor(.red)
            
            Text(message)
                .multilineTextAlignment(.center)
                .foregroundColor(.secondary)
            
            Button(action: retryAction) {
                HStack {
                    Image(systemName: "arrow.clockwise")
                    Text("Try Again")
                }
                .padding()
                .background(Color.blue)
                .foregroundColor(.white)
                .cornerRadius(10)
            }
        }
        .padding()
    }
}
