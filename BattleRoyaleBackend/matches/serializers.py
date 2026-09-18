from rest_framework import serializers
from .models import MatchRecord, MatchParticipant

class MatchParticipantSerializer(serializers.ModelSerializer):
    username = serializers.CharField(source='user.username', read_only=True)

    class Meta:
        model = MatchParticipant
        fields = ('id', 'username', 'placement_rank', 'kills', 'damage_dealt', 'xp_earned')

class MatchRecordSerializer(serializers.ModelSerializer):
    participants = MatchParticipantSerializer(many=True, read_only=True)

    class Meta:
        model = MatchRecord
        fields = ('id', 'match_id', 'map_id', 'total_players', 'duration_seconds', 'created_at', 'participants')
