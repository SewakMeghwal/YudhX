from django.urls import path
from .views import TopWinsLeaderboardView, TopKillsLeaderboardView

urlpatterns = [
    path('wins/', TopWinsLeaderboardView.as_view(), name='leaderboard_wins'),
    path('kills/', TopKillsLeaderboardView.as_view(), name='leaderboard_kills'),
]
