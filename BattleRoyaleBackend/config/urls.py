from django.contrib import admin
from django.urls import path, include

urlpatterns = [
    path('admin/', admin.site.urls),
    path('api/auth/', include('accounts.urls')),
    path('api/player/', include('players.urls')),
    path('api/matches/', include('matches.urls')),
    path('api/leaderboard/', include('leaderboards.urls')),
]
