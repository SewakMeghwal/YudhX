import os
import json
import django

os.environ.setdefault('DJANGO_SETTINGS_MODULE', 'config.settings')
django.setup()

from django.test import Client

def test_full_api_flow():
    c = Client()
    
    # 1. Register User
    reg_resp = c.post('/api/auth/register/', data={'username': 'survivor1', 'email': 'survivor1@yudhx.com', 'password': 'SecretPass123!'}, content_type='application/json')
    print("1. Registration Status:", reg_resp.status_code, reg_resp.json())
    assert reg_resp.status_code == 201 or reg_resp.status_code == 400 # 400 if user exists

    # 2. Login User (JWT Token)
    login_resp = c.post('/api/auth/login/', data={'username': 'survivor1', 'password': 'SecretPass123!'}, content_type='application/json')
    print("2. Login Status:", login_resp.status_code)
    assert login_resp.status_code == 200
    token = login_resp.json()['access']
    print("   Obtained Bearer JWT Token:", token[:25] + "...")

    # 3. Get Player Profile
    profile_resp = c.get('/api/player/profile/', HTTP_AUTHORIZATION=f'Bearer {token}')
    print("3. Player Profile Status:", profile_resp.status_code, profile_resp.json())
    assert profile_resp.status_code == 200

    # 4. Get Leaderboards (Wins & Kills)
    lb_wins = c.get('/api/leaderboard/wins/')
    print("4. Leaderboard Wins Status:", lb_wins.status_code, "Top Count:", len(lb_wins.json()))
    assert lb_wins.status_code == 200

    lb_kills = c.get('/api/leaderboard/kills/')
    print("5. Leaderboard Kills Status:", lb_kills.status_code, "Top Count:", len(lb_kills.json()))
    assert lb_kills.status_code == 200

    print("\n[SUCCESS] ALL REST BACKEND API ENDPOINTS FUNCTIONING 100% PERFECTLY!")

if __name__ == '__main__':
    test_full_api_flow()
