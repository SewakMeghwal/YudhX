from django.urls import path
from .views import PlayerProfileDetailView

urlpatterns = [
    path('profile/', PlayerProfileDetailView.as_view(), name='player_profile'),
]
