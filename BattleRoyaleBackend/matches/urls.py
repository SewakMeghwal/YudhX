from django.urls import path
from .views import MatchListCreateView, UserMatchHistoryView

urlpatterns = [
    path('', MatchListCreateView.as_view(), name='match_list_create'),
    path('history/', UserMatchHistoryView.as_view(), name='match_user_history'),
]
