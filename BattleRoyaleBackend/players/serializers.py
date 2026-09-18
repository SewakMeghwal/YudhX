from rest_framework import serializers
from .models import PlayerProfile

class PlayerProfileSerializer(serializers.ModelSerializer):
    username = serializers.CharField(source='user.username', read_only=True)
    kd_ratio = serializers.ReadOnlyField()
    win_rate = serializers.ReadOnlyField()

    class Meta:
        model = PlayerProfile
        fields = (
            'id', 'username', 'display_name', 'avatar_url', 'level',
            'total_xp', 'total_matches', 'total_wins', 'total_kills',
            'total_deaths', 'total_damage', 'kd_ratio', 'win_rate', 'updated_at'
        )
